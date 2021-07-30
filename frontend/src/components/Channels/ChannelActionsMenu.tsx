import {
  Button,
  Menu,
  MenuButton,
  MenuDivider,
  MenuGroup,
  MenuItem,
  MenuList,
  useBreakpointValue
} from "@chakra-ui/react";
import { BsBoxArrowInRight } from "@react-icons/all-files/bs/BsBoxArrowInRight";
import { AuthContext } from "contexts/AuthContext";
import { useRouter } from "next/router";
import React, { useContext } from "react";
import { FC } from "react";
import { IChannelSettingsIdDto } from "services/backend/nswagts";

import EditChannelSettingsTriggerBtn from "./EditChannelSettings/EditChannelSettingsTriggerBtn";

type Props = {
  channel: IChannelSettingsIdDto;
};
const ChannelActionsMenu: FC<Props> = ({ channel }) => {
  const { activeUser } = useContext(AuthContext);
  const router = useRouter();

  const text = useBreakpointValue({
    base: "",
    md: "Navigation Menu"
  });

  return (
    <Menu>
      <MenuButton
        as={Button}
        colorScheme="blue"
        leftIcon={<BsBoxArrowInRight />}
        aria-label="menu"
        m="auto"
        size="sm">
        {text}
      </MenuButton>

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
          <MenuItem
            onClick={() =>
              router.push(
                "channels/[channelId]/prizes/mine",
                "channels/" + channel.id + "/prizes/mine"
              )
            }>
            My Prizes
          </MenuItem>
        </MenuGroup>

        {activeUser?.channelsToAdmin.includes(channel.id) && (
          <>
            <MenuDivider />
            <MenuGroup title="Admin">
              <MenuItem>
                <EditChannelSettingsTriggerBtn channel={channel}>
                  Edit Channel Settings
                </EditChannelSettingsTriggerBtn>
              </MenuItem>
              <MenuItem
                onClick={() =>
                  router.push(
                    "channels/[channelId]/prizes/admin",
                    "channels/" + channel.id + "/prizes/admin"
                  )
                }>
                Edit Channel Prizes
              </MenuItem>
            </MenuGroup>
          </>
        )}
      </MenuList>
    </Menu>
  );
};

export default ChannelActionsMenu;
