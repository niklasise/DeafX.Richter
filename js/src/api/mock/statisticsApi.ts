import StatisticsPoint from "models/Statistics/StatisticsPoint";
import IStatisticsApi from "../interfaces/IStatisticsApi"

const DELAY: number = 1000;

class StatisticsApi implements IStatisticsApi {

    public getStatistics(id: string, from: number, to: number, minimumDataInterval: number): Promise<StatisticsPoint[]> {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(this.getRandomizedData(from, to, minimumDataInterval));
            }, DELAY);
        });
    }

    private getRandomizedData(from: number, to: number, interval: number): StatisticsPoint[] {

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