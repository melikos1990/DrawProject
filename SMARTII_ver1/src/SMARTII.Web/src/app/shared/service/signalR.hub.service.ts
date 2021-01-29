import { Observable, BehaviorSubject } from "rxjs";
import { Injectable } from "@angular/core";

import { SignalR, ISignalRConnection, BroadcastEventListener } from "ng2-signalr";
import { filter, take } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class SignalRHubService {

  private connection: ISignalRConnection;
  private connectionSubject$: BehaviorSubject<ISignalRConnection> = new BehaviorSubject<ISignalRConnection>(null);

  get isConnected(){ return this.connection === void 0 ? false : true; }

  constructor(private signalR: SignalR) { }

  /**
   * 初使化hub
   * 連線到server
   */
  async initialization(account: string) {
    console.log("------------------- SignalR Initialization ------------------------");
    await this.init(account);
  }

  private async init(account: string) {

    await this.connect(account)
      .then(conn => {
        this.connection = conn;
        this.connectionSubject$.next(conn);
      })
      .catch(err => {
        console.warn("signalr try reconnect");
        this.initialization(account);
      });

  }

  /**
   * 實際連線
   */
  public connect(account: string): Promise<ISignalRConnection> {

    return this.signalR.connect({ qs: { Account: account } });
  }

  /**
   * SERVER叫用Client
   */
  listener<T>(clientMethodName: string): Observable<T> {

    let clientMethod$ = new BroadcastEventListener<T>(clientMethodName);

    // 阻塞 , 值到 IConnection 被初始化
    this.connectionSubject$.pipe(
      filter(conn => conn != null),
      take(1)
    ).subscribe(conn => {
      console.log("取得signalr連線 ", conn);
      conn.listen(clientMethod$);
    });

    return <Observable<T>>clientMethod$;
  }

  /**
   * 叫用SERVER的方法
   * @param connection 目前的連線
   * @param serverMethodName 叫用的名稱
   * @param params 參數
   */
  invoker<T>(serverMethodName: string, ...params: any[]) {

    return this.connection
      .invoke(serverMethodName, params);
  }

  start(): Promise<any> {
    return this.connection === void 0
      ? null
      : this.connection.start();
  }

  stop(): void {

    if (this.connection === void 0) return;

    return this.connection.stop();
  }

  getConnection(): ISignalRConnection {
    return this.connection;
  }
}

