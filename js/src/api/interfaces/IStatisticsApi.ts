import StatisticsPoint from "models/Statistics/StatisticsPoint";

export default interface IStatisticsApi {

    getStatistics(id: string, from: number, to: number, minimumDataInterval: number) : Promise<StatisticsPoint[]>;

}