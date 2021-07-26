import { Box, Grid, GridItem, HStack, Skeleton, Spacer, useColorModeValue } from "@chakra-ui/react";
import { MdShoppingCart } from "@react-icons/all-files/md/MdShoppingCart";
import { useHover } from "hooks/useHover";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useState } from "react";
import { useCallback } from "react";
import { ClaimPrizeForUserCommand, IPrizeIdDTO, PrizesClient } from "services/backend/nswagts";

interface Props {
  prize: IPrizeIdDTO;
  isPurchasable?: boolean;
  addCallback?: (claimed: boolean) => Promise<void> | void;
}

const AvailablePrize: FC<Props> = ({ prize, isPurchasable = true, addCallback = () => null }) => {
  const [hoverRef, isHovered] = useHover<HTMLDivElement>();
  const [loading, setLoading] = useState(false);

  const { genClient } = useNSwagClient(PrizesClient);

  const bgColor = useColorModeValue("green.200", "green.600");

  const claimPrize = useCallback(async () => {
    setLoading(true);
    const client = await genClient();

    const result = await client
      .claimPrizeForUser(
        new ClaimPrizeForUserCommand({
          prizeId: prize.id
        })
      )
      .catch(() => false);

    await addCallback(result);
    setLoading(false);
  }, []);

  return (
    <Grid templateRows="5fr 7fr" templateColumns="3fr 1fr">
      <GridItem colSpan={1} p={1}>
        <p>
          <b>{prize.title}</b>
        </p>
      </GridItem>
      <GridItem colSpan={1} bg={bgColor} borderRadius={4}>
        {loading ? (
          <Skeleton height="100%" />
        ) : (
          <Box
            ref={hoverRef}
            onClick={claimPrize}
            onKeyPress={claimPrize}
            tabIndex={-1}
            role={isPurchasable && "button"}
            p={1}>
            <HStack>
              {isPurchasable && isHovered ? (
                <p>
                  Click to buy prize for {prize.pointCost}
                  <i>p</i>
                </p>
              ) : (
                <p>
                  <b>Cost:</b> {prize.pointCost}
                  <i>p</i>
                </p>
              )}
              <Spacer />
              {isPurchasable && <MdShoppingCart />}
            </HStack>
          </Box>
        )}
      </GridItem>
      <GridItem colSpan={2} p={1}>
        <p>{prize.description}</p>
      </GridItem>
    </Grid>
  );
};

export default AvailablePrize;
