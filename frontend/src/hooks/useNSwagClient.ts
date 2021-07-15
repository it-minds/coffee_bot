import { AuthContext } from "contexts/AuthContext";
import { useCallback, useContext } from "react";
import { api, NSwagClient } from "services/backend/api";
import { ClientBase } from "services/backend/nswagts";

interface NSwagClientType<T> {
  genClient: () => Promise<T>;
}

export const useNSwagClient = <
  T,
  V extends NSwagClient<ClientBase & T> = NSwagClient<ClientBase & T>
>(
  client: V
): NSwagClientType<T> => {
  const { checkAuth } = useContext(AuthContext);

  const genClient = useCallback(async () => {
    const initializedClient = await api(client);

    initializedClient.setStatusCallbackMap({
      401: () => {
        console.info("nswag 401 - checking auth");
        checkAuth();
      }
    });
    return initializedClient;
  }, []);

  return {
    genClient
  };
};
