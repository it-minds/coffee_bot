import { HStack, Text } from "@chakra-ui/react";
import { useRouter } from "next/dist/client/router";
import React, { FC, ReactNode, useContext } from "react";

import { BreadcrumbContext } from "./BreadcrumbContext";

const Divider = () => (
  <Text mr={4} ml={4}>
    {"/"}
  </Text>
);

const Breadcrumbs: FC = () => {
  const { breadcrumbs } = useContext(BreadcrumbContext);
  const router = useRouter();

  return (
    <HStack divider={<Divider />} mr={4} ml={4}>
      {breadcrumbs.map<ReactNode>(x => (
        <Text cursor="pointer" key={x.name} onClick={() => router.push(x.path, x.asPath)}>
          {x.name}
        </Text>
      ))}
    </HStack>
  );
};

export default Breadcrumbs;
