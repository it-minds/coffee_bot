import { Dispatch, useCallback, useEffect, useReducer } from "react";
import ListReducer, { AllListActions, ListReducerActionType } from "react-list-reducer";
import { genChannelClient } from "services/backend/apiClients";
import {
  IChannelSettingsIdDto,
  UpdateChannelPauseCommand,
  UpdateChannelPauseInput
} from "services/backend/nswagts";
import { logger } from "utils/logger";
type ChannelHook = {
  fetchChannels: () => Promise<void>;
  channels: IChannelSettingsIdDto[];
  dispatchChannels: Dispatch<AllListActions<IChannelSettingsIdDto>>;
  updateChannelPaused: (input: UpdateChannelPauseInput) => Promise<void>;
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

  const updateChannelPaused = useCallback(async (input: UpdateChannelPauseInput) => {
    try {
      const client = await genChannelClient();
      await client.updateChannelState(new UpdateChannelPauseCommand({ input: input }));
    } catch (err) {
      logger.warn("ApplicationClient.getAppToken Error", err);
    } finally {
      fetchChannels();
    }
  }, []);

  useEffect(() => {
    fetchChannels();
  }, []);

  return { fetchChannels, channels, dispatchChannels, updateChannelPaused };
};
