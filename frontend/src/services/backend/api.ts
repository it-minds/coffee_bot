/* istanbul ignore file */
import { getAuthToken } from "hooks/useAuth";
import fetch from "isomorphic-unfetch";
import { GetServerSidePropsContext } from "next";
import isomorphicEnvSettings, { setEnvSettings } from "utils/envSettings";

// !NOTE: If you are having build errors with this file missing, the backend is required to be built first
import { ClientBase, ClientConfiguration } from "./nswagts";

export interface NSwagClient<T extends ClientBase> {
  new (
    configuration: ClientConfiguration,
    baseUrl?: string,
    http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> },
    ...args: unknown[]
  ): T;
}

export const api = async <T extends ClientBase, V extends NSwagClient<T>>(
  Client: V,
  context?: GetServerSidePropsContext
): Promise<InstanceType<V>> => {
  let envSettings = isomorphicEnvSettings();

  if (envSettings === null && process.browser) {
    envSettings = await fetch("/api/getEnv").then(res => res.json());
    setEnvSettings(envSettings);
  }
  if (envSettings === null && !process.browser) {
    throw new Error("Environment settings null on server");
  }

  const authToken = getAuthToken(context) ?? "";
  const initializedClient = new Client(new ClientConfiguration(authToken), envSettings.backendUrl, {
    fetch
  }) as InstanceType<V>;

  return initializedClient;
};
