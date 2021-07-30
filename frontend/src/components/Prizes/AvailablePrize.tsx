import { Grid, GridItem } from "@chakra-ui/react";
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

  return (
    <Grid templateRows="5fr 7fr" templateColumns="1fr 7fr 2fr">
      <GridItem rowSpan={2}>
        <CupImage w={20} filter={isClaimed ? "" : "grayscale(100%)"} opacity={0.8} />
      </GridItem>
      <GridItem colSpan={1} p={1}>
        <p>
          <b>{prize.title}</b>
        </p>
      </GridItem>
      <GridItem colSpan={1}>{children}</GridItem>
      <GridItem colSpan={2} p={1}>
        <p>{prize.description}</p>
      </GridItem>
    </Grid>
  );
};

export default AvailablePrize;
