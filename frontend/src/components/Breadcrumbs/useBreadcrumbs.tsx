import { useContext, useEffect } from "react";
import { ParamHook } from "types/Hook";

import { Breadcrumb } from "./Breadcrumb";
import { BreadcrumbContext } from "./BreadcrumbContext";

export const useBreadcrumbs: ParamHook<void, Breadcrumb[]> = (breadcrumbs, deps = []) => {
  const { setBreadcrumbs } = useContext(BreadcrumbContext);

  useEffect(() => {
    setBreadcrumbs(breadcrumbs);
  }, [...deps]);

  return;
};
