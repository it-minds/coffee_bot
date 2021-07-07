import {
  Flex,
  IconButton,
  Popover,
  PopoverArrow,
  PopoverBody,
  PopoverContent,
  PopoverTrigger,
  Portal,
  Spacer,
  Switch,
  Text
} from "@chakra-ui/react";
import { BsThreeDots } from "@react-icons/all-files/bs/BsThreeDots";
import EditChannelSettingsTriggerBtn from "components/Channels/EditChannelSettings/EditChannelSettingsTriggerBtn";
import { ChannelContext } from "contexts/ChannelContext";
import React, { FC, useContext } from "react";
import { IChannelSettingsIdDto, UpdateChannelPauseInput } from "services/backend/nswagts";

type Props = {
  channel: IChannelSettingsIdDto;
};
const ChannelListItem: FC<Props> = ({ channel }) => {
  const { updateChannelPaused } = useContext(ChannelContext);
  if (!channel) return null;
  return (
    <Flex rounded="lg" w="m" h="40px" justify="center" align="center" borderWidth={2} m="10px">
      <Text m="3">{channel.slackChannelName}</Text>
      <Spacer />
      <Switch
        size="sm"
        isChecked={!channel.paused}
        onChange={() => {
          updateChannelPaused(
            new UpdateChannelPauseInput({
              channelId: channel.id,
              paused: !channel.paused
            })
          );
        }}
      />
      <Popover placement="right">
        <PopoverTrigger>
          <IconButton icon={<BsThreeDots />} aria-label="menu"></IconButton>
        </PopoverTrigger>
        <Portal>
          <PopoverContent>
            <PopoverArrow />
            <PopoverBody p="0">
              <EditChannelSettingsTriggerBtn channel={channel} />
            </PopoverBody>
          </PopoverContent>
        </Portal>
      </Popover>
    </Flex>
  );
};
export default ChannelListItem;
