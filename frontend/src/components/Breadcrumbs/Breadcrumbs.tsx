import { HStack, Text, useBreakpointValue } from "@chakra-ui/react";
import { useRouter } from "next/dist/client/router";
import React, { FC, ReactNode, useContext } from "react";
import { useMemo } from "react";

import { Breadcrumb } from "./Breadcrumb";
import { BreadcrumbContext } from "./BreadcrumbContext";

const Divider = () => (
  <Text mr={[1, 2, 4]} ml={[1, 2, 4]}>
    {"/"}
  </Text>
);

const middle = {
  name: "...",
  path: null,
  asPath: null
} as Breadcrumb;

const Breadcrumbs: FC = () => {
  const { breadcrumbs } = useContext(BreadcrumbContext);
  const router = useRouter();

  const maxCount = useBreakpointValue({
    base: 2,
    sm: 3,
    md: 4,
    lg: 6,
    xl: 8
  });

  const hasMiddle = useMemo(() => breadcrumbs.length > maxCount, [breadcrumbs, maxCount]);

  return (
    <HStack divider={<Divider />} mr={[1, 2, 4]} ml={[1, 2, 4]}>
      {(hasMiddle
        ? [breadcrumbs[0], middle, breadcrumbs[breadcrumbs.length - 1]]
        : breadcrumbs
      ).map<ReactNode>(x => (
        <Text cursor="pointer" key={x.name} onClick={() => x.path && router.push(x.path, x.asPath)}>
          {x.name}
        </Text>
      ))}
    </HStack>
  );
};

export default Breadcrumbs;
