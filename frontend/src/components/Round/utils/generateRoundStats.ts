import { ActiveRoundDto } from "services/backend/nswagts";
import { percentFormatter } from "utils/formatters/percentFormatter";

import { StatsProps } from "../StatsBox";

export const generateRoundStats = (round: ActiveRoundDto): StatsProps[] => {
  const meetup =
    (round.coffeeRoundGroups.filter(x => x.hasMet).length * 100) / round.coffeeRoundGroups.length;
  const photo =
    (round.coffeeRoundGroups.filter(x => x.hasPhoto).length * 100) /
    (round.coffeeRoundGroups.filter(x => x.hasMet).length || 1);

  const score = round.coffeeRoundGroups.sum(x => x.groupScore);

  return [
    {
      title: "Total Score",
      current: score,
      previous: round.previousScore,
      formatter: (x: number) => x + "p"
    },
    {
      title: "Meetup rate",
      current: meetup,
      previous: round.previousMeetup,
      formatter: (x: number) => percentFormatter.format(x)
    },
    {
      title: "Photo rate",
      current: photo,
      previous: round.previousPhoto,
      formatter: (x: number) => percentFormatter.format(x)
    }
  ];
};
