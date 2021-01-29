import { getLogger, Logger, BrowserConsoleAppender, SimpleLayout, AjaxAppender, JsonLayout } from 'log4javascript'
import { Injectable } from '@angular/core';


@Injectable({
    providedIn: 'root',
})
export class LogService {

    public logger: Logger
    static _instance: LogService;


    constructor() {
        if (this.logger == null) {
            this.initialize();
        }
    }

    initialize() {
        this.logger = getLogger('demo');

        // BrowserConsoleAppender
        const consoleAppender = new BrowserConsoleAppender();
        const consoleLayout = new ConsoleLayout();

        consoleAppender.setLayout(consoleLayout);

        // AjaxAppender
        const ajaxAppender = new AjaxAppender('App/Logger'.toHostApiUrl());
        const jsonLayout = new JsonLayout();
        //jsonLayout.setCustomField('user' , 'user');
        ajaxAppender.setLayout(jsonLayout);
        ajaxAppender.setWaitForResponse(false);
        ajaxAppender.setRequestSuccessCallback(this.ajaxSuccessCallback);
        ajaxAppender.addHeader('content-type' , 'application/json');

        this.logger.addAppender(consoleAppender);
        //this.logger.addAppender(ajaxAppender);
    }


    ajaxSuccessCallback(httpRequest: XMLHttpRequest) {

        if (httpRequest.status != 200) {
            console.error(`上傳logger失敗 , status : ${httpRequest.status}`);
        }
    }

}

export class ConsoleLayout extends SimpleLayout{

}




