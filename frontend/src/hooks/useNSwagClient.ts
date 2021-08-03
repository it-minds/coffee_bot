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

const genNSwagClient = async <T extends ClientBase, V extends NSwagClient<T> = NSwagClient<T>>(
  Client: V,
  checkAuth: () => Promise<void>,
  abortController = new IsomorphicAbortController()
) => {
  const initializedClient = await api(Client);

  initializedClient.setStatusCallbackMap({
    401: async res => {
      console.info("nswag 401 - checking auth");
      await checkAuth();
      return res.json();
    }
  });
  initializedClient.setAbortSignal(abortController.signal);

  return initializedClient;
};

export const useNSwagClient = <T extends ClientBase, V extends NSwagClient<T> = NSwagClient<T>>(
  client: V,
  useAbortOnUnmount = false
): NSwagClientType<InstanceType<V>> => {
  const { checkAuth } = useContext(AuthContext);
  const abortController = useRef(new IsomorphicAbortController());

  const genClient = useCallback(
    () => genNSwagClient(client, checkAuth, abortController.current),
    []
  );

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
