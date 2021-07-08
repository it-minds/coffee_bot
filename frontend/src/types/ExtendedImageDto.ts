import { StandardGroupDto } from "services/backend/nswagts";

export type ExtendedImageDto = StandardGroupDto & {
  publicSrc: string;
};
