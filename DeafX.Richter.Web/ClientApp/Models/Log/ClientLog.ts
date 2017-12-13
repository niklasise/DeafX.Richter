export interface ClientLog {
    timestamp: number;
    message: string,
    file: string,
    line: number,
    column: number,
    errorStack: string
}
