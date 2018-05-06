import StatisticsPoint from "models/Statistics/StatisticsPoint";

let delay: number = 1000;

class StatisticsApi {


    static getStatistics(id: string, from: number, to: number, minimumDataInterval: number): Promise<StatisticsPoint[]> {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(StatisticsApi.getRandomizedData(from, to, minimumDataInterval));
            }, delay);
        });
    }

    private static getRandomizedData(from: number, to: number, interval: number): StatisticsPoint[] {

        let data: StatisticsPoint[] = [];
        let value = Math.random() * 5 + 15;

        for (let time = from; time <= to; time += interval)
        {
            value += Math.random() * 6 - 3;

            data.push({
                timeStamp: time,
                data: value
            })
        }

        return data;
    }

}

export default StatisticsApi;