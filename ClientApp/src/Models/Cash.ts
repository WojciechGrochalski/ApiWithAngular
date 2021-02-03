export class Cash{

  Name: string;
  Code: string;
  BidPrice: number;
 AskPrice: number;
 Data: string;

  constructor(name: string, code: string, bidPrice: number, askPrice: number, data: string) {
    this.Name = name;
    this.Code = code;
    this.BidPrice = bidPrice;
    this.AskPrice = askPrice;
    this.Data = data;

  }
}
