export class VerifyEmailRequest{

  Token:string;

  constructor(token: string) {
    this.Token = token;
  }
}
