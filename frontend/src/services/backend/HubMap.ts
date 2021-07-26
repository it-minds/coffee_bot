import { IPrizeIdDTO } from "./nswagts";

export type HubMap = {
  prize: {
    NewPrize: IPrizeIdDTO;
    PrizeDeleted: [id: number, time: Date];
  };
};
