
import { SignalRConfiguration } from "ng2-signalr";
import { environment } from 'src/environments/environment';


export function config(): SignalRConfiguration {

    const config = new SignalRConfiguration();

    config.hubName = "SignalRHub";
    config.url = environment.apHost;
    config.logging = true;
    config.withCredentials = false;

    return config;

}
