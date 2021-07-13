import { GetServerSidePropsContext } from "next";

import { api } from "./api";
import { AuthClient, ChannelClient, GalleryClient, RoundClient, StatsClient } from "./nswagts";

export const genAuthenticationClient = (context?: GetServerSidePropsContext): Promise<AuthClient> =>
  api(AuthClient, context);
export const genChannelClient = (context?: GetServerSidePropsContext): Promise<ChannelClient> =>
  api(ChannelClient, context);
export const genGalleryClient = (context?: GetServerSidePropsContext): Promise<GalleryClient> =>
  api(GalleryClient, context);

export const genStatsClient = (context?: GetServerSidePropsContext): Promise<StatsClient> =>
  api(StatsClient, context);
export const genRoundsClient = (context?: GetServerSidePropsContext): Promise<RoundClient> =>
  api(RoundClient, context);
