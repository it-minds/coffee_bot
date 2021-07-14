import { createContext } from "react";

import { Breadcrumb } from "./Breadcrumb";

export interface BreadcrumbContextType {
  setBreadcrumbs: (breadcrumbs: Breadcrumb[]) => void;
  addBreadcrumbs: (breadcrumb: Breadcrumb | Breadcrumb[]) => void;
  breadcrumbs: Breadcrumb[];
}

export const BreadcrumbContext = createContext<BreadcrumbContextType>({
  addBreadcrumbs: () => null,
  breadcrumbs: [],
  setBreadcrumbs: () => null
});
