import StatisticsPoint from "models/Statistics/StatisticsPoint";

class StatisticsApi {

    static getStatistics(deviceId: string, from: number, to: number, minimumDataInterval: number): Promise<StatisticsPoint[]> {
        return new Promise<StatisticsPoint[]>((resolve, reject) => {
            fetch(`/api/statistics/${deviceId}?from=${from}&to=${to}&minimumDataInterval=${minimumDataInterval}`, {
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