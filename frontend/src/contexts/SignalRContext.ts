import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { getAuthToken } from "hooks/useAuth";
import { createContext } from "react";
import { HubMap } from "services/backend/HubMap";
import isomorphicEnvSettings from "utils/envSettings";

export class MyHub<T extends keyof HubMap> {
  private constructor(private connection: HubConnection) {}

  static async startConnection<T extends keyof HubMap>(hub: T): Promise<MyHub<T>> {
    if (!process.browser) return null;

    const envSettings = isomorphicEnvSettings();
    const token = getAuthToken();
    console.log(token);
    const connection = new HubConnectionBuilder()
      .withUrl(envSettings.backendUrl + "/hubs/" + hub, {
        headers: {
          Authorization: "bearer " + token
        },
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    await connection.start();

    return new MyHub<T>(connection);
  }

  public onConnect<U extends keyof HubMap[T]>(key: U, cb: (args: HubMap[T][U]) => void): void {
    this.connection.on(key.toString(), cb);
  }

  public getConnection(): HubConnection {
    return this.connection;
  }
}

export const PrizeSignalRContext = createContext<MyHub<"prize">>(null);
