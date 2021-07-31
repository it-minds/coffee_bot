import { Grid, GridItem, useBreakpointValue } from "@chakra-ui/react";
import CupImage from "components/Images/CupImage";
import React, { FC } from "react";
import { IPrizeIdDTO } from "services/backend/nswagts";

interface Props {
  prize: IPrizeIdDTO;
  isClaimed?: boolean;
}

const AvailablePrize: FC<Props> = ({ prize, isClaimed = false, children }) => {
  // if (!isPFC(children)) {
  //   console.log(children._type);
  //   throw Error("Tree structure require PFC");
  // }

  const breakLower = useBreakpointValue({
    base: true,
    md: false
  });

  return (
    <Grid templateRows="5fr 7fr" templateColumns="1fr 7fr 2fr">
      <GridItem rowSpan={breakLower ? 1 : 2}>
        <CupImage w={20} minW={16} filter={isClaimed ? "" : "grayscale(100%)"} opacity={0.8} />
      </GridItem>
      <GridItem p={1}>
        <p>
          <b>{prize.title}</b>
        </p>
      </GridItem>
      <GridItem>{children}</GridItem>
      <GridItem colSpan={breakLower ? 3 : 2} p={1}>
        <p>{prize.description}</p>
      </GridItem>
    </Grid>
  );
};

export default AvailablePrize;
