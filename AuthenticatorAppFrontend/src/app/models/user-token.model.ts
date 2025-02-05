import { IAutoMapper } from "../core/automapper/interfaces";
import { StringHelper } from "../utilities/contract/string-helper";

export const mapUserToken = (automapper: IAutoMapper): void => {
     automapper
    .createMap("default", "UserToken")
    .forMember("userID", (o:any) => o.userID)
    .forMember("userName", (o:any) => o.userName)
    .forMember("fullUserName", (o:any) => o.fullUserName)
    .forMember("is2FA", (o:any) => o.is2FA)
};

export class UserToken {
  userID: number;
  userName: string;
  fullUserName: string;
  is2FA:boolean;

  constructor(options: {
    userID?: number;
    userName?: string;
    fullUserName?: string;
    is2FA?: boolean;
  } = {}) {

    this.userID = options.userID !== undefined ? options.userID : 0;
    this.userName = options.userName !== undefined ? StringHelper.clean(options.userName) : "";
    this.fullUserName = options.fullUserName !== undefined ? options.fullUserName : "";;
    this.is2FA = options.is2FA !== undefined ? options.is2FA : false;;
  }

  get isAuthenticated(): boolean {
    return StringHelper.isAvailable(this.userName) && this.is2FA;
  }
}
