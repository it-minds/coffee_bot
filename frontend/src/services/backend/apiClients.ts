import { GetServerSidePropsContext } from "next";

import { api } from "./api";
import { AuthClient, ChannelClient } from "./nswagts";

export const genAuthenticationClient = (context?: GetServerSidePropsContext): Promise<AuthClient> =>
  api(AuthClient, context);
export const genChannelClient = (context?: GetServerSidePropsContext): Promise<ChannelClient> =>
  api(ChannelClient, context);
