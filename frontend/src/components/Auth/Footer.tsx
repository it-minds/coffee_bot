import { Button, Text } from "@chakra-ui/react";
import { useRouter } from "next/router";
import React, { FC } from "react";

const Footer: FC = () => {
  const router = useRouter();

  return (
    <Text fontWeight="md" m={8} textAlign="center">
      <Button variant="ghost" onClick={() => router.push("/")}>
        Go to app.
      </Button>
      <Button
        as="a"
        variant="ghost"
        href={`mailto:contact@it-minds.dk?subject="Coffee Buddies Web: "`}>
        Contact us.
      </Button>
      <Button variant="ghost" onClick={() => router.push("/info")}>
        Info.
      </Button>
    </Text>
  );
};

export default Footer;
