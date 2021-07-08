import { GetServerSidePropsContext } from "next";

import { api } from "./api";
import { AuthClient, ChannelClient, GalleryClient, StatsClient } from "./nswagts";

export const genAuthenticationClient = (context?: GetServerSidePropsContext): Promise<AuthClient> =>
  api(AuthClient, context);
export const genChannelClient = (context?: GetServerSidePropsContext): Promise<ChannelClient> =>
  api(ChannelClient, context);
export const genGalleryClient = (context?: GetServerSidePropsContext): Promise<GalleryClient> =>
  api(GalleryClient, context);

export const genStatsClient = (context?: GetServerSidePropsContext): Promise<StatsClient> =>
  api(StatsClient, context);
