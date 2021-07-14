import { useState } from "react";
import { SimpleHook } from "types/Hook";

import { Breadcrumb } from "./Breadcrumb";
import { BreadcrumbContextType } from "./BreadcrumbContext";

export const useBreadcrumbsContext: SimpleHook<BreadcrumbContextType> = () => {
  const [breadcrumbs, setBreadcrumbs] = useState<Breadcrumb[]>([]);

  return {
    addBreadcrumbs: () => null,
    breadcrumbs,
    setBreadcrumbs
  };
};
