import { StatusCode } from "../enums/status-code.enum";

export class APIResponse<T> {
    Data!: [T];
    statusCode!: StatusCode;
    result!: T;
}