import { useChannelContext } from "hooks/useChannelContext";
import { createContext } from "react";

type ContextType = ReturnType<typeof useChannelContext>;

export const ChannelContext = createContext<ContextType>({
  channels: [],
  dispatchChannels: null,
  fetchChannels: () => null,
  updateChannelPaused: () => null,
  updateChannelSettings: () => null
});
