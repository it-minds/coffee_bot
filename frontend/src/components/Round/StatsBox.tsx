import {
  Icon,
  Stat,
  StatArrow,
  StatGroup,
  StatHelpText,
  StatLabel,
  StatNumber,
  useColorModeValue
} from "@chakra-ui/react";
import { CgMathEqual } from "@react-icons/all-files/cg/CgMathEqual";
import React, { ReactText } from "react";
import { FC } from "react";

export interface StatsProps {
  title: string;
  current: number;
  previous: number;
  formatter?: (x: number) => ReactText;
}

const StatsMaker: FC<StatsProps> = ({ current, formatter = x => x, previous, title }) => {
  return (
    <Stat p={[2, 4]} minW={32}>
      <StatLabel>{title}</StatLabel>
      <StatNumber>
        {current == previous ? (
          <Icon w={4} color={useColorModeValue("yellow.400", "yellow.400")} as={CgMathEqual} />
        ) : (
          <StatArrow type={current > previous ? "increase" : "decrease"} />
        )}
        {formatter(current)}
      </StatNumber>
      <StatHelpText>
        <i>previous: {formatter(previous)}</i>
      </StatHelpText>
    </Stat>
  );
};

interface Props {
  pairs: StatsProps[];
}

const StatsBox: FC<Props> = ({ pairs }) => {
  return (
    <StatGroup
      mt={6}
      mb={3}
      backgroundColor={useColorModeValue("blue.200", "blue.700")}
      borderRadius={10}
      align="center">
      {pairs.map(x => (
        <StatsMaker
          key={x.title}
          title={x.title}
          current={x.current}
          previous={x.previous}
          formatter={x.formatter}
        />
      ))}
    </StatGroup>
  );
};

export default StatsBox;
