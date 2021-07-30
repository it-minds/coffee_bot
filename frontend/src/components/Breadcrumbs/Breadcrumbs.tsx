import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  Text,
  useBreakpointValue
} from "@chakra-ui/react";
import { useRouter } from "next/dist/client/router";
import React, { FC, ReactNode, useContext } from "react";
import { useMemo } from "react";

// import { Breadcrumb } from "./Breadcrumb";
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
} as any;

const Breadcrumbs: FC = () => {
  const { breadcrumbs } = useContext(BreadcrumbContext);
  const router = useRouter();

  const maxCount = useBreakpointValue({
    base: 2,
    sm: 3,
    md: 5,
    lg: 6,
    xl: 8
  });

  const crumbs = useMemo(() => {
    if (breadcrumbs.length < 1) return [];

    const middleCount = breadcrumbs.length - maxCount;

    return middleCount < 1
      ? breadcrumbs
      : [breadcrumbs[0], middle, ...breadcrumbs.filter((_, i) => i > middleCount)];
  }, [breadcrumbs, maxCount]);

  return (
    <Breadcrumb mr={[1, 2, 4]} ml={4}>
      {crumbs.map<ReactNode>((x, i) => (
        <BreadcrumbItem key={x.name} isCurrentPage={crumbs.length - 1 == i}>
          <BreadcrumbLink onClick={() => x.path && router.push(x.path, x.asPath)}>
            {x.name}
          </BreadcrumbLink>
        </BreadcrumbItem>
      ))}
    </Breadcrumb>
  );
};

export default Breadcrumbs;
