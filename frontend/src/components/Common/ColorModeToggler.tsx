import { IconButton, useColorMode } from "@chakra-ui/react";
import { FaMoon } from "@react-icons/all-files/fa/FaMoon";
import { FaSun } from "@react-icons/all-files/fa/FaSun";
import React, { FC } from "react";

const ColorModeToggler: FC = () => {
  const { colorMode, toggleColorMode } = useColorMode();

  return (
    <IconButton
      onClick={toggleColorMode}
      aria-label="light / dark toggle"
      colorScheme="gray"
      icon={colorMode === "light" ? <FaMoon color="Black" /> : <FaSun color="white" />}
    />
  );
};
export default ColorModeToggler;
