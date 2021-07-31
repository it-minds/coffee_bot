import { Button, Text } from "@chakra-ui/react";
import ColorModeToggler from "components/Common/AppContainer/ColorModeToggler";
import { useRouter } from "next/router";
import React, { FC } from "react";

const Footer: FC = () => {
  const router = useRouter();

  return (
    <Text fontWeight="md" m={8} textAlign="center">
      <Button variant="ghost" onClick={() => router.push("/")}>
        Go to app.
      </Button>
      <Button variant="ghost" onClick={() => router.push("/info")}>
        Info.
      </Button>
      <Button as="a" variant="ghost" href={`mailto:contact@it-minds.dk?subject="Coffee Buddies: "`}>
        Contact us.
      </Button>
      <ColorModeToggler />
    </Text>
  );
};

export default Footer;
