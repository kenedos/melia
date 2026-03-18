"""
For monster skill handlers with multiple SkillAttack calls,
swaps hitDelay and aniTime values for all hits AFTER the first.

The first hit keeps its original values (already correct).
Subsequent hits have their hitDelay/aniTime swapped to match Option A logic.
"""

import os
import re
import glob

HANDLERS_DIR = r"E:\Melia\Melia-Laima4\melia\src\ZoneServer\Skills\Handlers\Monsters"

def fix_file(filepath):
	with open(filepath, "r", encoding="utf-8") as f:
		content = f.read()

	original = content

	handler_pattern = re.compile(
		r'\[SkillHandler\(SkillId\.(\w+)\)\].*?'
		r'(?:private|public)\s+async\s+Task\s+HandleSkill\b.*?\{(.*?)\n\t\}',
		re.DOTALL
	)

	changes = []

	for match in handler_pattern.finditer(original):
		skill_id_name = match.group(1)
		method_body = match.group(2)

		# Find all SkillAttack calls
		attack_count = len(re.findall(r'SkillAttack\(', method_body))
		if attack_count <= 1:
			continue

		# Find all hitDelay/aniTime REASSIGNMENTS (not var declarations)
		# These are the subsequent hits after the first
		reassign_pattern = re.compile(
			r'(\t+)(hitDelay = )(\d+)(;\s*\n\s*)(aniTime = )(\d+)(;)'
		)

		reassigns = list(reassign_pattern.finditer(method_body))
		if not reassigns:
			continue

		swapped = []
		for m in reassigns:
			hit_val = m.group(3)
			ani_val = m.group(6)
			if hit_val != ani_val:
				swapped.append((hit_val, ani_val))

		if not swapped:
			continue

		# Do the swaps in the full content
		# We need to replace within this specific method body
		method_start = match.start(2)
		method_end = match.end(2)
		new_body = method_body

		for m in reversed(reassigns):
			hit_val = m.group(3)
			ani_val = m.group(6)
			if hit_val == ani_val:
				continue
			# Swap: hitDelay gets old aniTime, aniTime gets old hitDelay
			old_text = m.group(0)
			new_text = f"{m.group(1)}hitDelay = {ani_val}{m.group(4)}aniTime = {hit_val}{m.group(7)}"
			new_body = new_body[:m.start()] + new_text + new_body[m.end():]

		if new_body != method_body:
			content = content[:method_start] + new_body + content[method_end:]
			changes.append({
				"skill": skill_id_name,
				"swaps": len(swapped),
			})

	if content != original:
		with open(filepath, "w", encoding="utf-8") as f:
			f.write(content)

	return changes

def main():
	print("Fixing multi-hit monster handlers (swapping subsequent hit delays)...")
	cs_files = glob.glob(os.path.join(HANDLERS_DIR, "**", "*.cs"), recursive=True)

	total_changes = 0
	total_files = 0

	for cs_file in sorted(cs_files):
		changes = fix_file(cs_file)
		if changes:
			total_files += 1
			rel_path = os.path.relpath(cs_file, HANDLERS_DIR)
			for c in changes:
				print(f"  {c['skill']:<50} {c['swaps']} hits swapped  ({rel_path})")
				total_changes += 1

	print()
	print(f"Done: {total_changes} multi-hit handlers updated across {total_files} files")

if __name__ == "__main__":
	main()
