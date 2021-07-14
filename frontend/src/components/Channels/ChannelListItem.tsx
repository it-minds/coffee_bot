import {
  Button,
  IconButton,
  Menu,
  MenuButton,
  MenuDivider,
  MenuGroup,
  MenuItem,
  MenuList,
  Switch,
  Td,
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
      <Td pl={[1, 2, 4]} pr={[1, 2, 4]} pt={1} pb={1} m="3">
        #{channel.slackChannelName}
      </Td>
      <Td pl={[1, 2, 4]} pr={[1, 2, 4]} pt={1} pb={1} textAlign="center">
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
      <Td pl={[1, 2, 4]} pr={[1, 2, 4]} pt={1} pb={1}>
        <Menu>
          <MenuButton
            as={IconButton}
            colorScheme="blue"
            icon={<BsThreeDots />}
            aria-label="menu"
            m="auto"
            size="sm"
          />

          {/* <Portal> */}
          <MenuList maxW="90vw">
            {/* <PopoverArrow /> */}

            <MenuGroup title="General">
              <MenuItem
                onClick={() =>
                  router.push("channels/[channelId]/gallery", "channels/" + channel.id + "/gallery")
                }>
                Gallery
              </MenuItem>
              <MenuItem
                onClick={() =>
                  router.push("channels/[channelId]/stats", "channels/" + channel.id + "/stats")
                }>
                Stats
              </MenuItem>
              <MenuItem
                onClick={() =>
                  router.push(
                    "channels/[channelId]/rounds/active",
                    "channels/" + channel.id + "/rounds/active"
                  )
                }>
                Active Round
              </MenuItem>
              <MenuItem
                onClick={() =>
                  router.push("channels/[channelId]/rounds", "channels/" + channel.id + "/rounds")
                }>
                Previous Rounds
              </MenuItem>
            </MenuGroup>
            <MenuDivider />
            <MenuGroup title="Admin">
              <MenuItem>
                <EditChannelSettingsTriggerBtn channel={channel}>
                  Edit Channel Settings
                </EditChannelSettingsTriggerBtn>
              </MenuItem>
            </MenuGroup>
          </MenuList>
        </Menu>
      </Td>
    </Tr>
  );
};
export default ChannelListItem;
