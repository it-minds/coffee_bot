import { Context, createContext } from "react";
import { AllHubs } from "services/backend/nswagts";

import { AuthorizedHub } from "./AuthorizedHub";

type KeysEnum<T> = { [P in keyof Required<T>]: true };
const allHubs: KeysEnum<AllHubs> = {
  prize: true
};

type ContextMapType<T extends keyof AllHubs = keyof AllHubs> = Record<T, Context<AuthorizedHub<T>>>;

export const allHubContexts = Object.freeze(
  Object.keys(allHubs).reduce<ContextMapType>((acc, cur: keyof AllHubs) => {
    acc[cur] = createContext<AuthorizedHub<"prize">>(null);
    return acc;
  }, {} as any)
);

export const getHubContext = <T extends keyof AllHubs>(key: T): Context<AuthorizedHub<T>> => {
  return (allHubContexts as unknown as ContextMapType<T>)[key];
};
