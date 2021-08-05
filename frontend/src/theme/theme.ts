import { ColorMode, extendTheme } from "@chakra-ui/react";

const config = {
  initialColorMode: "dark" as ColorMode,
  useSystemColorMode: false
};

const colors = {
  brown: {
    "50": "#F5F2F0",
    "100": "#E3DAD4",
    "200": "#D0C3B8",
    "300": "#BEAB9D",
    "400": "#AC9481",
    "500": "#9A7C65",
    "600": "#7B6351",
    "700": "#5C4B3D",
    "800": "#3E3228",
    "900": "#1F1914"
  }
};

const theme = extendTheme({ config, colors });
export default theme;
