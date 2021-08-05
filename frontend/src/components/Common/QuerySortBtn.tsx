import { IconButton, IconButtonProps } from "@chakra-ui/react";
import { FcAlphabeticalSortingAz } from "@react-icons/all-files/Fc/FcAlphabeticalSortingAz";
import { FcAlphabeticalSortingZa } from "@react-icons/all-files/Fc/FcAlphabeticalSortingZa";
import { MdSort } from "@react-icons/all-files/md/MdSort";
import { useRouter } from "next/router";
import { useRef } from "react";
import { FC, useCallback, useEffect, useMemo, useState } from "react";

export type Direction = "ASC" | "DESC";

const sortMap: Record<Direction | "null", Direction[]> = {
  ASC: ["DESC", null, "ASC"],
  DESC: ["ASC", null, "DESC"],
  null: ["ASC", "DESC", null]
};

type Props = {
  queryKey: string;
  queryGroup?: string;
  sortCb?: (group: string, key: string, direction: Direction) => void;
  defaultDirection?: Direction;
};

const QuerySortBtn: FC<Props & Partial<IconButtonProps>> = ({
  sortCb = () => null,
  queryKey,
  queryGroup = "t",
  defaultDirection = null,
  ...rest
}) => {
  const [direction, setDirection] = useState<Direction>(defaultDirection);
  const active = useMemo(() => direction !== null, [direction]);

  const { query, replace } = useRouter();
  const queryCopy = useRef(query);

  useEffect(() => {
    console.log("resetting query", query);
    queryCopy.current = query;
  }, [query]);

  useEffect(() => {
    sortCb(queryGroup, queryKey, direction as Direction);
  }, [queryGroup, queryKey, direction]);

  const firstOrDefaultDirection = useCallback(
    (key: string): Direction | null => {
      const sort = key?.split("_");

      if (sort && sort.length === 3) {
        const [checkQueryGroup, checkkey, direction] = sort;

        if (queryGroup === checkQueryGroup) {
          if (queryKey === checkkey) {
            return direction as Direction;
          }
        }
      }
      return null;
    },
    [queryGroup, queryKey]
  );

  useEffect(() => {
    const thsort = queryCopy.current.thsort;

    if (Array.isArray(thsort)) {
      let myDirection: Direction = null;
      thsort.some(x => {
        const result = firstOrDefaultDirection(x);
        if (result !== null) {
          myDirection = result;
          return true;
        }
      });
      setDirection(myDirection);
    }

    if (typeof thsort === "string") {
      setDirection(firstOrDefaultDirection(thsort));
    }
  }, []);

  const getNextDirection = useCallback(() => {
    const sortArr = sortMap[defaultDirection ?? "null"];
    const curIndex = sortArr.findIndex(x => x == direction);
    return curIndex + 1 >= sortArr.length ? sortArr[0] : sortArr[curIndex + 1];
  }, [defaultDirection, direction]);

  const setQuery = useCallback((newDirection: string) => {
    const getQuery = (newDirection: string) => {
      const key = `${queryGroup}_${queryKey}_${newDirection}`;
      const thsort = queryCopy.current.thsort;

      if (Array.isArray(thsort)) {
        const index = thsort.findIndex(x => firstOrDefaultDirection(x) !== null);
        if (newDirection === null) {
          delete thsort[index];
        } else {
          thsort[index] = key;
        }
        return thsort;
      }

      if (
        typeof thsort === "string" &&
        firstOrDefaultDirection(thsort) === null &&
        newDirection !== null
      ) {
        return [thsort, key];
      }

      if (newDirection === null) return thsort;

      return key;
    };

    const thsort = getQuery(newDirection);

    console.log(queryCopy.current, newDirection, thsort);
    queryCopy.current.thsort = thsort;
    replace({ query: queryCopy.current }, undefined, { shallow: true });
  }, []);

  useEffect(() => {
    if (defaultDirection !== null) setQuery(defaultDirection);
  }, [defaultDirection]);

  const onClick = useCallback(() => {
    const newDirection = getNextDirection();
    setDirection(newDirection);
    setQuery(newDirection);
  }, [setQuery, getNextDirection]);

  return (
    <IconButton
      size="sm"
      aria-label="Sort column"
      onClick={onClick}
      isActive={active}
      icon={
        active ? (
          direction === "ASC" ? (
            <FcAlphabeticalSortingAz size={20} />
          ) : (
            <FcAlphabeticalSortingZa size={20} />
          )
        ) : (
          <MdSort />
        )
      }
      {...rest}
    />
  );
};

export default QuerySortBtn;
