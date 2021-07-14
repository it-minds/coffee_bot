import { Box } from "@chakra-ui/react";
import React, { FC } from "react";
import { IconType } from "react-icons";

interface Props {
  isDisable?: boolean;
  onClick: () => void;
  icon: IconType;
  size?: number;
}

const PureIconsButton: FC<Props> = ({ icon, onClick, isDisable = false, size = 6 }) => (
  <Box
    opacity={isDisable ? 0.1 : 1}
    as={icon}
    boxSize={size}
    cursor={isDisable ? "no-drop" : "pointer"}
    onClick={isDisable ? () => null : onClick}
  />
);

export default PureIconsButton;
