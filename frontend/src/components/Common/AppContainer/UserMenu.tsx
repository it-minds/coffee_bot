import { Box, Menu, MenuItem, MenuList, Spacer } from "@chakra-ui/react";
import { AuthContext } from "contexts/AuthContext";
import React, { FC } from "react";
import { useContext } from "react";

import AppVersion from "./AppVersion";
import ColorModeToggler from "./ColorModeToggler";
import UserMenuToggle from "./UserMenuToggle";

const UserMenu: FC = () => {
  const { logout, activeUser } = useContext(AuthContext);

  return (
    <Menu>
      <UserMenuToggle>{activeUser.email}</UserMenuToggle>
      <MenuList>
        <MenuItem onClick={logout}>Logout</MenuItem>
        <MenuItem as={Box}>
          <AppVersion>v. {process.env.NEXT_PUBLIC_APP_VERSION}</AppVersion>
          <Spacer />
          <ColorModeToggler />
        </MenuItem>
      </MenuList>
    </Menu>
  );
};
export default UserMenu;
