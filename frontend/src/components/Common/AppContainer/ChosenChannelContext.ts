import { createContext } from "react";

export interface ChosenChannelContextType {
  chosenChannel: {
    id: number;
    name: string;
  };
}

export const ChosenChannelContext = createContext<ChosenChannelContextType>({
  chosenChannel: null
});
