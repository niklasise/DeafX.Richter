import StatisticsPoint from "models/Statistics/StatisticsPoint";
import IStatisticsApi from "./interfaces/IStatisticsApi";
import ConfigurationUtlity from "utilities/configurationUtility";

class StatisticsApi implements IStatisticsApi{

    private _apiBaseUrl : string;

    constructor() {
        this._apiBaseUrl = ConfigurationUtlity.getConfiguration().apiUrl; 
    }

    public getStatistics(deviceId: string, from: number, to: number, minimumDataInterval: number): Promise<StatisticsPoint[]> {
        return new Promise<StatisticsPoint[]>((resolve, reject) => {
            fetch(`${this._apiBaseUrl}/statistics/${deviceId}?from=${from}&to=${to}&minimumDataInterval=${minimumDataInterval}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                }
            }).then(response => {
                if (response.ok) {           
                    response.json().then(data => resolve(data)).catch(error => { reject() });
                }
                else {
                    reject(Error("Server request responded with status code: " + response.status));
                }
            }).catch(error => {
                reject(error);
            });

        });
    }

}

export default StatisticsApi;