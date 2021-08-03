import { useEffectAsync } from "hooks/useEffectAsync";
import { Provider, useEffect, useMemo } from "react";
import { useState } from "react";
import { AllHubs } from "services/backend/nswagts";

import { AuthorizedHub } from "./AuthorizedHub";
import { getHubContext } from "./utils";

type Hook = <T extends keyof AllHubs>(
  key: T,
  autoCloseOnUnmount?: boolean
) => {
  hub: AuthorizedHub<T>;
  Provider: Provider<AuthorizedHub<T>>;
  isConnected: boolean;
};

export const useHubProvider: Hook = <T extends keyof AllHubs>(
  key: T,
  autoCloseOnUnmount = false
) => {
  const [hub, setHub] = useState<AuthorizedHub<T>>(null);

  const HubProvider = useMemo(() => getHubContext(key), [key]);

  useEffectAsync(async () => {
    const hub = await AuthorizedHub.startConnection(key);
    setHub(hub);
  }, [key]);

  useEffect(() => {
    if (hub && autoCloseOnUnmount) {
      return () => {
        hub.closeConnection();
      };
    }
  }, [hub]);

  const isConnected = useMemo(() => !!hub?.getConnection()?.connectionId, [hub]);

  return {
    hub,
    Provider: HubProvider.Provider,
    isConnected
  };
};
