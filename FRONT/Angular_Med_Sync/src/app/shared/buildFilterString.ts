export function buildFilterString(filters: { [key: string]: any }): string {
  return Object.entries(filters)
    .filter(([_, value]) => value !== null && value !== undefined && value !== '')
    .map(([key, value]) => `${key}:${value}`)
    .join(';');
}