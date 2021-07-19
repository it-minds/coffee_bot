import { AuthContext } from "contexts/AuthContext";
import { useCallback, useContext, useEffect, useRef } from "react";
import { api, NSwagClient } from "services/backend/api";
import { ClientBase } from "services/backend/nswagts";
import IsomorphicAbortController from "utils/isomorphicAbortController";

interface NSwagClientType<T> {
  genClient: () => Promise<T>;
  abortController: AbortController;
  refreshAbortSignal: () => void;
}

export const useNSwagClient = <
  T,
  V extends NSwagClient<ClientBase & T> = NSwagClient<ClientBase & T>
>(
  client: V,
  useAbortOnUnmount = false
): NSwagClientType<T> => {
  const { checkAuth } = useContext(AuthContext);
  const abortController = useRef(new IsomorphicAbortController());

  const genClient = useCallback(async () => {
    const initializedClient = await api(client);

    initializedClient.setStatusCallbackMap({
      401: async res => {
        console.info("nswag 401 - checking auth");
        await checkAuth();
        return res.json();
      }
    });
    initializedClient.setAbortSignal(abortController.current.signal);

    return initializedClient;
  }, []);

  const refreshAbortSignal = useCallback(() => {
    abortController.current = new IsomorphicAbortController();
  }, []);

  useEffect(() => {
    if (useAbortOnUnmount) {
      abortController.current.signal.addEventListener("abort", () => {
        console.log("aborted on unmount!");
      });

      return () => {
        abortController.current.abort();
      };
    }
  }, [useAbortOnUnmount]);

  return {
    genClient,
    abortController: abortController.current,
    refreshAbortSignal
  };
};
