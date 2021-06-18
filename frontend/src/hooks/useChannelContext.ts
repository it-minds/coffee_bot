import { Dispatch, useCallback, useEffect, useReducer } from "react";
import ListReducer, { AllListActions, ListReducerActionType } from "react-list-reducer";
import { genChannelClient } from "services/backend/apiClients";
import { IChannelSettingsIdDto } from "services/backend/nswagts";
import { logger } from "utils/logger";
type ChannelHook = {
  fetchChannels: () => Promise<void>;
  channels: IChannelSettingsIdDto[];
  dispatchChannels: Dispatch<AllListActions<IChannelSettingsIdDto>>;
};
export const useChannelContext = (): ChannelHook => {
  const [channels, dispatchChannels] = useReducer(ListReducer<IChannelSettingsIdDto>("id"), []);

  const fetchChannels = useCallback(async () => {
    try {
      const client = await genChannelClient();
      const data = await client.getMyChannels();
      if (data && data.length >= 0) {
        dispatchChannels({
          type: ListReducerActionType.Reset,
          data
        });
      } else logger.info("ApplicationClient.getAppTokensICanReview got no data");
    } catch (err) {
      logger.warn("ApplicationClient.getAppToken Error", err);
    }
  }, []);

  useEffect(() => {
    fetchChannels();
  }, []);

  return { fetchChannels, channels, dispatchChannels };
};
