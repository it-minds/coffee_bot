import {
  Button,
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
  Td,
  Text,
  Tr
} from "@chakra-ui/react";
import { BsThreeDots } from "@react-icons/all-files/bs/BsThreeDots";
import EditChannelSettingsTriggerBtn from "components/Channels/EditChannelSettings/EditChannelSettingsTriggerBtn";
import { ChannelContext } from "contexts/ChannelContext";
import { useRouter } from "next/dist/client/router";
import React, { FC, useContext } from "react";
import { IChannelSettingsIdDto, UpdateChannelPauseInput } from "services/backend/nswagts";

const MyButton: FC<{ onClick: () => void }> = ({ onClick, children }) => (
  <Button
    justifyContent="left"
    isFullWidth={true}
    size="md"
    borderRadius={0}
    variant="ghost"
    onClick={onClick}>
    {children}
  </Button>
);

type Props = {
  channel: IChannelSettingsIdDto;
};
const ChannelListItem: FC<Props> = ({ channel }) => {
  const { updateChannelPaused } = useContext(ChannelContext);
  const router = useRouter();

  if (!channel) return null;
  return (
    <Tr rounded="lg" p={0} justify="center" align="center" m="10px">
      <Td pt={1} pb={1} m="3">
        #{channel.slackChannelName}
      </Td>
      <Td pt={1} pb={1} textAlign="center">
        <Switch
          size="md"
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
      </Td>
      <Td pt={1} pb={1} isNumeric>
        <Popover placement="right">
          <PopoverTrigger>
            <IconButton colorScheme="blue" icon={<BsThreeDots />} aria-label="menu"></IconButton>
          </PopoverTrigger>
          <Portal>
            <PopoverContent>
              <PopoverArrow />
              <PopoverBody p="0">
                <EditChannelSettingsTriggerBtn channel={channel} />
                <MyButton
                  onClick={() =>
                    router.push(
                      "channels/[channelId]/gallery",
                      "channels/" + channel.id + "/gallery"
                    )
                  }>
                  Gallery
                </MyButton>
                <MyButton
                  onClick={() =>
                    router.push("channels/[channelId]/stats", "channels/" + channel.id + "/stats")
                  }>
                  Stats
                </MyButton>
                <MyButton
                  onClick={() =>
                    router.push(
                      "channels/[channelId]/active-round",
                      "channels/" + channel.id + "/active-round"
                    )
                  }>
                  Active Round
                </MyButton>
              </PopoverBody>
            </PopoverContent>
          </Portal>
        </Popover>
      </Td>
    </Tr>
  );
};
export default ChannelListItem;
