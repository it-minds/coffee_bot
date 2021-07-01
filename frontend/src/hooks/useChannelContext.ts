import { AuthContext } from "contexts/AuthContext";
import { Dispatch, useCallback, useContext, useEffect, useReducer } from "react";
import ListReducer, { AllListActions, ListReducerActionType } from "react-list-reducer";
import { genChannelClient } from "services/backend/apiClients";
import {
  IChannelSettingsDto,
  IChannelSettingsIdDto,
  IUpdateChannelPauseInput,
  UpdateChannelPauseCommand,
  UpdateChannelSettingsCommand
} from "services/backend/nswagts";
import { logger } from "utils/logger";
type ChannelHook = {
  fetchChannels: () => Promise<void>;
  channels: IChannelSettingsIdDto[];
  dispatchChannels: Dispatch<AllListActions<IChannelSettingsIdDto>>;
  updateChannelPaused: (input: IUpdateChannelPauseInput) => Promise<void>;
  updateChannelSettings: (id: number, input: IChannelSettingsDto) => Promise<void>;
};
export const useChannelContext = (): ChannelHook => {
  const { activeUser } = useContext(AuthContext);

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
      } else logger.info("ChannelClient.getMyChannels got no data");
    } catch (err) {
      logger.warn("ChannelClient.getMyChannels Error", err);
    }
  }, []);

  const updateChannelPaused = useCallback(async (input: IUpdateChannelPauseInput) => {
    try {
      const client = await genChannelClient();
      await client.updateChannelState(new UpdateChannelPauseCommand({ input: input }));
    } catch (err) {
      logger.warn("ChannelClient.UpdateChannelState Error", err);
    } finally {
      fetchChannels();
    }
  }, []);

  const updateChannelSettings = useCallback(async (id: number, input: IChannelSettingsDto) => {
    try {
      const client = await genChannelClient();
      await client.updateChannelSettings(id, new UpdateChannelSettingsCommand({ settings: input }));
    } catch (err) {
      logger.warn("ChannelClient.updateChannelSettings Error", err);
    } finally {
      fetchChannels();
    }
  }, []);

  useEffect(() => {
    if (activeUser !== null) fetchChannels();
  }, [activeUser]);

  return { fetchChannels, channels, dispatchChannels, updateChannelPaused, updateChannelSettings };
};
