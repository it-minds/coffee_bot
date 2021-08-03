import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { getAuthToken } from "hooks/useAuth";
import { AllHubs } from "services/backend/nswagts";
import isomorphicEnvSettings from "utils/envSettings";

type Wrap = AllHubs;

export class AuthorizedHub<T extends keyof Wrap> {
  private constructor(private connection: HubConnection) {}

  static async startConnection<T extends keyof Wrap>(hub: T): Promise<AuthorizedHub<T>> {
    if (!process.browser) return null;

    const envSettings = isomorphicEnvSettings();
    const token = getAuthToken();
    console.log(token);
    const connection = new HubConnectionBuilder()
      .withUrl(envSettings.backendUrl + "/hubs/" + hub, {
        headers: {
          Authorization: "bearer " + token
        },
        withCredentials: false,
        accessTokenFactory: () => "bearer " + token
      })
      .withAutomaticReconnect()
      .build();

    await connection.start();

    return new AuthorizedHub<T>(connection);
  }

  public onConnect<U extends keyof Wrap[T]>(key: U, cb: (args: Wrap[T][U]) => void): void {
    this.connection.on(key.toString(), cb);
  }

  public getConnection(): HubConnection {
    return this.connection;
  }

  public closeConnection(): Promise<void> {
    return this.connection.stop();
  }
}
