import { GetServerSidePropsContext } from "next";

import { api } from "./api";
import { AuthClient } from "./nswagts";

export const genAuthenticationClient = (context?: GetServerSidePropsContext): Promise<AuthClient> =>
  api(AuthClient, context);
